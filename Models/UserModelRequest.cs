namespace PokemonMlEvalWebApp.Models;

public class UserModelRequest
{
    public int Quantity {get; set;}
    public int RandomState {get; set;}

    public UserModelRequest(){}

    public UserModelRequest(int quantity, int randomState)
    {
        Quantity = quantity;
        RandomState = randomState;
    }
}