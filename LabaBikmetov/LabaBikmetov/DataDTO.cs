namespace LabaBikmetov
{
    public class DataDTO
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public Guid idRequest { get; set; } = Guid.NewGuid();

        public int idUser { get; set; } = -1;

        public string? Comment { get; set; } = "";

        public DataDTO(int userId, string comment = "") {
            idUser = userId;
            Comment = comment;  
        }

        public override string ToString() { 
            return string.Join(";", Date, idRequest, idUser, Comment) + '\n';
        }
    }
}
