using FluentAssertions;
using Vote.Models;

namespace Vote.Steps;

[Binding]
public class VoteSteps
{
    private Scrutin _scrutin;
    private ResultatScrutin _resultat;
    private Exception _exception;

    [Given(@"un scrutin avec les candidats suivants:")]
    public void GivenUnScrutinAvecLesCandidatsSuivants(Table table)
    {
        _scrutin = new Scrutin
        {
            NumeroTour = 1
        };
        
        foreach (var row in table.Rows)
        {
            _scrutin.Candidats.Add(new Candidat 
            { 
                Nom = row["Nom"], 
                NombreVote = 0 
            });
        }
    }

    [Given(@"les votes suivants ont été enregistrés:")]
    public void GivenLesVotesSuivantsOntEteEnregistres(Table table)
    {
        foreach (var row in table.Rows)
        {
            if (row["Candidat"].ToLower().Contains("blanc"))
            {
                _scrutin.NombreVotesBlancs += int.Parse(row["Votes"]);
                continue;
            }
            var candidat = _scrutin.Candidats.First(c => c.Nom == row["Candidat"]);
            candidat.NombreVote = int.Parse(row["Votes"]);
        }
    }
    
    [Given(@"le scrutin est au deuxième tour")]
    public void GivenLeScrutinEstAuDeuxiemeTour()
    {
        _scrutin.NumeroTour = 2;
    }

    [When(@"le scrutin est clôturé")]
    public void WhenLeScrutinEstCloture()
    {
        _scrutin.IsCloture = true;
        _resultat = _scrutin.CalculerResultat();
    }

    [When(@"je tente de calculer le résultat sans clôturer le scrutin")]
    public void WhenJeTenteDeCalculerLeResultatSansCloturerLeScrutin()
    {
        try
        {
            _resultat = _scrutin.CalculerResultat();
        }
        catch (Exception ex)
        {
            _exception = ex;
        }
    }

    [Then(@"(.*) est déclarée? vainqueur")]
    public void ThenEstDeclareVainqueur(string nomVainqueur)
    {
        _resultat.Vainqueur.Should().Be(nomVainqueur);
        _resultat.Status.Should().Be("Gagnant");
    }

    [Then(@"le résultat affiche:")]
    public void ThenLeResultatAffiche(Table table)
    {
        for (int i = 0; i < table.Rows.Count; i++)
        {
            var expectedRow = table.Rows[i];
            var actualCandidat = _resultat.Candidats[i];
            
            actualCandidat.Nom.Should().Be(expectedRow["Candidat"]);
            actualCandidat.NombreVote.Should().Be(int.Parse(expectedRow["Votes"]));
            actualCandidat.Pourcentage
                .Should()
                .BeApproximately(double.Parse(expectedRow["Pourcentage"].Replace("%", "")), 0.001);
        }
    }

    [Then(@"une erreur est levée avec le message ""(.*)""")]
    public void ThenUneErreurEstLeveeAvecLeMessage(string messageAttendu)
    {
        _exception.Should().NotBeNull();
        _exception.Message.Should().Be(messageAttendu);
    }
    
    [Then(@"le statut du résultat est ""(.*)""")]
    public void ThenLeStatutDuResultatEst(string statutAttendu)
    {
        _resultat.Status.Should().Be(statutAttendu);
    }

}