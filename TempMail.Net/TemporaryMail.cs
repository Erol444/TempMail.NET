using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TempMail.Models;

namespace TempMail
{
    public class TemporaryMail : IDisposable
    {
        private readonly HttpClient client;
        public TemporaryMail()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://www.1secmail.com");
        }

        /// <summary>
        /// Generates a new mailbox
        /// </summary>
        /// <returns>Email address of the mailbox</returns>
        public async Task<string> GenerateRandomMailbox() =>
            (await GenerateRandomMailboxes(1))[0];

        /// <summary>
        /// Generates new mailboxes
        /// </summary>
        /// <param name="count">Number of mailboxes to generate</param>
        /// <returns>List of email address' of mailboxes</returns>
        public async Task<List<string>> GenerateRandomMailboxes(int count)
        {
            var res = await client.GetAsync($"api/v1/?action=genRandomMailbox&count={count}");
            res.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<List<string>>(await res.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Check the mailbox for new messages
        /// </summary>
        /// <param name="mail">Email address of the mailbox</param>
        /// <returns>List of all messages inside the mailbox</returns>
        public async Task<List<TempMailMessage>> CheckMailbox(string mail)
        {
            var parsed = mail.Split('@');
            var res = await client.GetAsync($"api/v1/?action=getMessages&login={parsed[0]}&domain={parsed[1]}");
            res.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<List<TempMailMessage>>(await res.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Waits for an email inside a mailbox that suffice the specified requirements
        /// </summary>
        /// <param name="mail">Email address of the mailbox</param>
        /// <param name="predicate">Specified requirements of the email</param>
        /// <returns>Email that suffice the specified requirements</returns>
        public async Task<TempMailEmail> WaitForEmail(string mail, Func<TempMailMessage, bool> predicate)
        {
            while (true)
            {
                await Task.Delay(5000); // 5 sec delay
                var mailbox = await CheckMailbox(mail);

                var message = mailbox.FirstOrDefault(predicate);
                if (message == null) continue;

                return await FetchEmail(mail, message.id);
            }
        }

        /// <summary>
        /// Fetches the email from the id
        /// </summary>
        /// <param name="mail">Email address of the mailbox</param>
        /// <param name="emailId">Email id</param>
        /// <returns></returns>
        public async Task<TempMailEmail> FetchEmail(string mail, int emailId)
        {
            var parsed = mail.Split('@');
            var res = await client.GetAsync($"api/v1/?action=readMessage&login={parsed[0]}&domain={parsed[1]}&id={emailId}");
            res.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<TempMailEmail>(await res.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Dispose the HttpClient
        /// </summary>
        public void Dispose()
        {
            client.Dispose();
        }
    }
}
