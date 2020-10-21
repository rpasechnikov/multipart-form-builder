using MultipartFormDataBuilder.Tests.Enums;
using System.Collections.Generic;

namespace MultipartFormDataBuilder.Tests.ViewModel
{
    public class BasicViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsValid { get; set; }
        public SelectOptionViewModel<Colour> PrimaryColour { get; set; }
        public ICollection<SelectOptionViewModel<Colour>> AdditionalColours { get; set; }
    }
}
