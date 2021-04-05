namespace HAF.Security
{
    public interface IRsaKeyProvider
    {
        string GetPrivateAndPublicKey();
        string GetPublicKey();
    }
}