namespace CakesByRAndRCore.Models
{
    public class AdminMenu
    {

        public List<ParentMenu> Menu { get; set; }

    }

    public class ParentMenu
    {
        public int Id { get; set; }
        public string TextEng { get; set; }
        public string URL { get; set; }
        public int? ParentId { get; set; }
        public int? Order { get; set; }
        public int? Status { get; set; }
        public List<ParentMenu> ChildMenus { get; set; }
    }


}
