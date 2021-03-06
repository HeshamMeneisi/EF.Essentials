namespace EF.Essentials.Encryption
{
    public interface ISignatureKeyContainer
    {
        SigningKey Key { get; set; }
    }

    public class SignatureKeyContainer : ISignatureKeyContainer
    {
       public SigningKey Key { get; set; }
    }
}
