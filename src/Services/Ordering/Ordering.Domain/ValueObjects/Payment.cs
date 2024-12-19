namespace Ordering.Domain.ValueObjects;

public class Payment
{
    public string? CardName { get; } = default!;

    public string CardNumber { get; } = default!;

    public string ExpirationDate { get; } = default!;

    public string CvvCode { get; } = default!;

    public int PaymentMethod { get; } = default!;

    protected Payment()
    {
    }

    private Payment(string cardName, string cardNumber, string expirationDate, string cvvCode, int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        ExpirationDate = expirationDate;
        CvvCode = cvvCode;
        PaymentMethod = paymentMethod;
    }


    public static Payment Of(string cardName, string cardNumber, string expirationDate, string cvvCode,
        int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvvCode);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvvCode.Length, 3);

        return new Payment(cardName, cardNumber, expirationDate, cvvCode, paymentMethod);
    }
}