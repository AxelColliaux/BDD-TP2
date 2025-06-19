namespace Vote.Models;

public class ResultatScrutin
{
    public string Vainqueur { get; set; }
    public int NumeroTour { get; set; }
    public string Status { get; set; }
    public List<Candidat> Candidats { get; set; }
}