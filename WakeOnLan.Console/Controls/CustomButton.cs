namespace WakeOnLan.Console.Controls
{
    public class CustomButton : Button
    {
        // クリック済かの判定
        private bool _Clicked = false;
        public bool Clicked
        {
            get => _Clicked;
            set
            {
                if (_Clicked != value)
                {
                    _Clicked = value;
                    // ボタンの色を変更するためにUseVisualStyleBackColorを切り替える
                    UseVisualStyleBackColor = !Clicked;
                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            // クリック済みではないときのみクリックイベントを実行
            if (!Clicked) { base.OnClick(e); }
        }
    }
}
