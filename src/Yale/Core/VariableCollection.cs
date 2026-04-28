using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Yale.Core.Interfaces;

namespace Yale.Core;

public sealed class VariableCollection
    : INotifyPropertyChanged,
        IEnumerable<KeyValuePair<string, object>>
{
    private static readonly MethodInfo GetVariableValueInternalOpenMethod =
        typeof(VariableCollection).GetMethod(
            nameof(GetVariableValueInternal),
            BindingFlags.Public | BindingFlags.Instance
        )
        ?? throw new MissingMethodException(
            typeof(VariableCollection).FullName,
            nameof(GetVariableValueInternal)
        );

    private static readonly ConcurrentDictionary<Type, MethodInfo> GetVariableValueClosedMethods =
        new();

    private readonly Dictionary<string, IVariable> values;

    public VariableCollection()
    {
        values = new Dictionary<string, IVariable>();
    }

    internal VariableCollection(StringComparer comparer)
    {
        values = new Dictionary<string, IVariable>(comparer);
    }

    public void Clear() => values.Clear();

    public int Count => values.Keys.Count;

    /// <summary>
    /// Adds a value to this compute instance. This can be referenced in expressions.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(string key, object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        values.Add(key, new Variable(value));
    }

    /// <summary>
    /// Returns the current value registered to a variable in this instance.
    /// </summary>
    /// <param name="key"></param>
    ///
    public object Get(string key) => values[key].ValueAsObject;

    /// <summary>
    /// Returns the current value registered to a variable in this instance.
    /// </summary>
    /// <param name="key"></param>
    public T Get<T>(string key) => (T)Get(key);

    public bool ContainsKey(string key) => values.ContainsKey(key);

    public bool Remove(string key) => values.Remove(key);

    public bool TryGetValue(string key, out object? value)
    {
        var success = values.TryGetValue(key, out var result);
        value = result?.ValueAsObject;
        return success && value is not null;
    }

    public bool TryGetValue<T>(string key, [NotNullWhen(true)] out T? value)
        where T : class
    {
        var success = values.TryGetValue(key, out var result);
        value = result as T;
        return success && value is not null;
    }

    public object this[string key]
    {
        get => values[key].ValueAsObject;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (values.TryGetValue(key, out var v) && v.Equals(value))
                return;

            values[key] = new Variable(value);

            //Todo: What is the point of this? The value has not been changed,
            //is has only been added to the variables collection
            if (value is INotifyPropertyChanged nValue)
            {
                nValue.PropertyChanged += (sender, args) =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(key));
                };
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(key));
        }
    }

    public ICollection<string> Keys => values.Keys;
    public ICollection<object> Values => values.Values.Select(v => v.ValueAsObject).ToList();

    internal bool TryGetCanonicalKey(string key, [NotNullWhen(true)] out string? canonicalKey)
    {
        foreach (var k in values.Keys)
        {
            if (values.Comparer.Equals(k, key))
            {
                canonicalKey = k;
                return true;
            }
        }
        canonicalKey = null;
        return false;
    }

    /// <summary>
    /// This is used to crate a method call that can retrieve a value from the value collection
    /// </summary>
    /// <param name="variableType">Return value</param>
    /// <returns></returns>
    internal static MethodInfo GetVariableLoadMethod(Type variableType) =>
        GetVariableValueClosedMethods.GetOrAdd(
            variableType,
            static t => GetVariableValueInternalOpenMethod.MakeGenericMethod(t)
        );

    public T GetVariableValueInternal<T>(string name) => (T)values[name].ValueAsObject;

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() =>
        new VariableEnumerator(values);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public event PropertyChangedEventHandler? PropertyChanged;
}
