class PokemonResponse:
    def __init__(self, id: int, name: str, type: str, health: int, defense: int, weight: int, attack: int, speed: int, 
                height: int, special_attack: int, special_defense: int, predicted_type: str, f1_score: float, accuracy: float, 
                recall: float, precision: float):
        self.Id: int = id
        self.Name: str = name
        self.Type: str = type
        self.Health: int = health
        self.Defense: int= defense
        self.Weight: int = weight
        self.Attack: int = attack
        self.Speed: int = speed
        self.Height: int = height
        self.SpecialAttack: int = special_attack
        self.SpecialDefense: int = special_defense

        self.PredictedType: str = predicted_type
        self.F1_Score: float = f1_score
        self.Accuracy: float = accuracy
        self.Recall: float = recall
        self.Precision: float = precision


