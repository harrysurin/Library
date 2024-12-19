namespace Library.ViewModels
{
    public class RentHistoryViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId {get; set;}
        public Guid BookId {get; set;}
        public DateTime DateOfRent {get; set;}
        public DateTime? DateOfReturn{get; set;}
    }
}