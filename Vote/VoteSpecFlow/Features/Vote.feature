Feature: Vote

    Background: 
        Given un scrutin avec les candidats suivants:
            | Nom        |
            | Brian      |
            | Noureddine |
            | Charles    |
            | Nil        |    

    @Majoritaire
    Scenario: Un candidat obtient plus de 50% des voix
        Given les votes suivants ont été enregistrés:
            | Candidat   | Votes |
            | Brian      | 60    |
            | Noureddine | 20    |
            | Charles    | 10    |
            | Nil        | 10    |
        When le scrutin est clôturé
        Then Brian est déclaré vainqueur
        And le résultat affiche:
            | Candidat   | Votes | Pourcentage |
            | Brian      | 60    | 60          |
            | Noureddine | 20    | 20          |
            | Charles    | 10    | 10          |
            | Nil        | 10    | 10          |
    
    @PasDeMajorite
    Scenario: Aucun candidat n'obtient la majorité
        Given les votes suivants ont été enregistrés:
            | Candidat   | Votes |
            | Brian      | 40    |
            | Noureddine | 30    |
            | Charles    | 20    |
            | Nil        | 10    |
        When le scrutin est clôturé
        Then le résultat affiche:
            | Candidat   | Votes | Pourcentage |
            | Brian      | 40    | 40          |
            | Noureddine | 30    | 30          |
        And le statut du résultat est "PasDeMajorite"
    
    @SecondTour
    Scenario: Deuxième tour avec les deux meilleurs candidats
        Given un scrutin avec les candidats suivants:
            | Nom        |
            | Brian      |
            | Noureddine |
        Given les votes suivants ont été enregistrés:
            | Candidat   | Votes |
            | Brian      | 45    |
            | Noureddine | 55    |
        When le scrutin est clôturé
        Then Noureddine est déclaré vainqueur
        And le résultat affiche:
            | Candidat   | Votes | Pourcentage |
            | Noureddine | 55    | 55          |
            | Brian      | 45    | 45          |
    
    @Egalite
    Scenario: Égalité au second tour
        Given un scrutin avec les candidats suivants:
            | Nom        |
            | Brian      |
            | Noureddine |
        And le scrutin est au deuxième tour
        Given les votes suivants ont été enregistrés:
            | Candidat   | Votes |
            | Brian      | 50    |
            | Noureddine | 50    |
        When le scrutin est clôturé
        Then le statut du résultat est "Egalite"
