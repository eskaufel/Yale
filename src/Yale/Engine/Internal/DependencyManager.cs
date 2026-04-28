using System.Buffers;

namespace Yale.Engine.Internal;

/// <summary>
/// Keeps track of expression dependencies
/// </summary>
internal sealed class DependencyManager
{
    /// <summary>
    /// Map of a node with edges
    /// </summary>
    private Dictionary<string, DependencyNode> Nodes { get; } = new();

    public void Clear() => Nodes.Clear();

    public void AddDependency(string expressionKey, string dependsOnKey)
    {
        DependencyNode? expressionNode = null;
        DependencyNode? dependsOnNode = null;

        if (Nodes.ContainsKey(dependsOnKey) == false)
        {
            dependsOnNode = new DependencyNode(dependsOnKey);
            Nodes.Add(dependsOnKey, dependsOnNode);
        }

        if (Nodes.ContainsKey(expressionKey) == false)
        {
            expressionNode = new DependencyNode(expressionKey);
            Nodes.Add(expressionKey, expressionNode);
        }

        dependsOnNode ??= Nodes[dependsOnKey];
        expressionNode ??= Nodes[expressionKey];

        expressionNode.AddPredecessor(dependsOnNode);
    }

    public string[] GetDirectDependents(string key)
    {
        if (!Nodes.TryGetValue(key, out var node))
            return Array.Empty<string>();
        return node.Dependents;
    }

    public DependentsResult GetDependents(string key)
    {
        var result = new DependentsResult(initialCapacity: 8);
        if (!Nodes.TryGetValue(key, out var node))
            return result;

        foreach (var pair in node.Dependents)
            GetDependentsRecursive(pair, ref result);

        return result;
    }

    private void GetDependentsRecursive(string nodeKey, ref DependentsResult dependents)
    {
        dependents.Add(nodeKey);
        foreach (var pair in Nodes[nodeKey].Dependents)
            GetDependentsRecursive(pair, ref dependents);
    }

    public string[] GetDirectPrecedents(string nodeKey) => Nodes[nodeKey].Precedents;

    public void RemovePrecedents(string nodeKey)
    {
        if (Nodes.TryGetValue(nodeKey, out var value))
        {
            value.ClearPredecessors();
        }
    }

    public string DependencyGraph
    {
        get
        {
            var lines = new string[Nodes.Count];
            var index = 0;
            foreach (var node in Nodes)
            {
                var key = node.Key;
                var dependencies = string.Join(",", node.Value.Dependents);
                lines[index] = $"{key} -> {dependencies}";
                index += 1;
            }
            return string.Join(Environment.NewLine, lines);
        }
    }

    public int DependencyNodes => Nodes.Count;
}

/// <summary>
/// ArrayPool-backed buffer for iterating dependents without heap allocation per call.
/// Must be disposed after use (use with `using var`).
/// </summary>
internal ref struct DependentsResult
{
    private string[] _buffer;
    private int _count;

    internal DependentsResult(int initialCapacity)
    {
        _buffer = ArrayPool<string>.Shared.Rent(initialCapacity > 0 ? initialCapacity : 1);
        _count = 0;
    }

    internal void Add(string item)
    {
        if (_count == _buffer.Length)
        {
            var grown = ArrayPool<string>.Shared.Rent(_buffer.Length * 2);
            _buffer.AsSpan(0, _count).CopyTo(grown);
            ArrayPool<string>.Shared.Return(_buffer, clearArray: false);
            _buffer = grown;
        }
        _buffer[_count++] = item;
    }

    public ReadOnlySpan<string>.Enumerator GetEnumerator()
    {
        ReadOnlySpan<string> span =
            _buffer is null ? ReadOnlySpan<string>.Empty : new ReadOnlySpan<string>(_buffer, 0, _count);
        return span.GetEnumerator();
    }

    public void Dispose()
    {
        if (_buffer is not null)
        {
            ArrayPool<string>.Shared.Return(_buffer, clearArray: true);
            _buffer = null!;
        }
    }
}
