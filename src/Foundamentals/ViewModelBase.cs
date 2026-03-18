namespace Foundamentals
{
    public abstract class ViewModelBase : ObservableObject
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                this.SetProperty<bool>(ref this._isBusy, value);
            }
        }
    }
}
