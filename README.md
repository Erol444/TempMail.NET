## .NET TempMail library
#### Based on [1secmail.com API](https://www.1secmail.com/)


##### Features
- Create new mailbox(es)
- Check the mailbox for new emails
- Fetch an email
- Wait for an email based on the specified requirement(s)

###### Add using statement
    using TempMail;

###### Code example

    async Task TemporaryEmailDemo()
    {
        using (var tempMail = new TemporaryMail())
        {
            var mailbox = await tempMail.GenerateRandomMailbox();

            // Some code eg. await registrationService.Register(mailbox);

            // Email address of the sender has to contain "example.com"
            // and email must not be older than 2 minutes
            var email = await tempMail.WaitForEmail(mailbox,
                x => x.from.Contains("example.com") && DateTime.Now < x.GetDate().AddMinutes(2)
                );

            // Some code eg. parse email body for the activation code / url

            Console.WriteLine(email.body);
        }
    }