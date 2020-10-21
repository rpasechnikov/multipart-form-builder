namespace MultipartFormDataBuilder.Tests.ViewModel
{
    //
    // Summary:
    //     Represents a single item from a dropdown (usually through dynamic forms)
    //
    // Type parameters:
    //   TKey:
    public class SelectOptionViewModel<TKey>
    {
        public TKey Id { get; set; }
        public string Name { get; set; }

        public SelectOptionViewModel() { }

        public SelectOptionViewModel(TKey id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
