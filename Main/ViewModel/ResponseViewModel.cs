namespace Main.ViewModel
{
    public class ResponseViewModel<T>
    {
        public T Data { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();

        public ResponseViewModel(T data)
        {
            Data = data;
        }

        public ResponseViewModel(T data, IList<string> errors) : this(data)
        {
            Data = data;
            Errors = errors;
        }

        public ResponseViewModel(IList<string> errors)
        {
            Errors = errors;
        }

        public ResponseViewModel(string error)
        {
            Errors.Add(error);
        }
    }
}
