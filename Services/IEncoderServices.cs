namespace Backend_Prog4.Services
{
    public interface IEncoderServices
    {
       
            string Encode(string toHash);
            bool Verify(string str, string hash);
        
    }
}
