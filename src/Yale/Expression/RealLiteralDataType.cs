namespace Yale.Expression
{
    /// <summary>
    /// Defines values to indicate the data type to use for storing real literals.
    /// </summary>
    public enum RealLiteralDataType
    {
#pragma warning disable CA1720 // Identifier contains type name

        /// <summary>
        /// Specifies that real literals will be stored using the <see cref="Single"/> data type.
        /// </summary>
        Single,

        /// <summary>
        /// Specifies that real literals will be stored using the <see cref="Double"/> data type.
        /// </summary>
        Double,

        /// <summary>
        /// Specifies that real literals will be stored using the <see cref="Decimal"/> data type.
        /// </summary>
        Decimal

#pragma warning restore CA1720 // Identifier contains type name
    }
}