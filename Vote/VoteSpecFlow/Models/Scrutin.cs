namespace Vote.Models;

public class Scrutin
{
    public bool IsCloture { get; set; }
    public List<Candidat> Candidats { get; set; } = new List<Candidat>();
    public int NumeroTour { get; set; }
    public int TotalVotes => Candidats.Sum(c => c.NombreVote);

    public ResultatScrutin CalculerResultat()
    {
        if (!IsCloture)
        {
            throw new Exception("Le scrutin doit être cloturé");
        }

        foreach (var candidat in Candidats)
        {
            candidat.Pourcentage = (double)candidat.NombreVote / TotalVotes * 100;
        }
        
        List<Candidat> candidatsTries = Candidats.OrderByDescending(c => c.Pourcentage).ToList();
        Candidat candidatMajoritaire = candidatsTries.FirstOrDefault(c => c.Pourcentage > 50);

        if (candidatMajoritaire != null)
        {
            return new ResultatScrutin
            {
                Candidats = candidatsTries,
                Status = "Gagnant",
                NumeroTour = NumeroTour,
                Vainqueur = candidatMajoritaire.Nom
            };
        }
        
        if (NumeroTour == 1)
        {
            return new ResultatScrutin
            {
                Candidats = candidatsTries.Take(2).ToList(),
                Status = "PasDeMajorite",
                NumeroTour = 2,
                Vainqueur = null
            };
        }
        else
        {
            if (candidatsTries.Count >= 2 && 
                candidatsTries[0].NombreVote == candidatsTries[1].NombreVote)
            {
                return new ResultatScrutin
                {
                    Candidats = candidatsTries,
                    Status = "Egalite",
                    NumeroTour = 2,
                    Vainqueur = null
                };
            }

            return new ResultatScrutin
            {
                Candidats = candidatsTries,
                Status = "Gagnant",
                NumeroTour = 2,
                Vainqueur = candidatsTries.First().Nom
            };
        }
    }
}