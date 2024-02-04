namespace WakeOnLan.Console.Extensions
{
    // バインドの拡張メソッド
    public static class BindingExtension
    {
        // バインドするメソッド
        public static Binding Bind(this Control _control, string _propertyName, object _dataSource, string _dataMember) =>
            _control.DataBindings.Add(_propertyName, _dataSource, _dataMember);

        // Bindされる値を変換
        public static void SetConveter(this Binding binding, Func<object, object> converter)
        {
            void ConvertMethod(object? sender, ConvertEventArgs e) => e.Value = converter(e.Value!);

            binding.Format += ConvertMethod;
            binding.Parse += ConvertMethod;
        }
    }
}
