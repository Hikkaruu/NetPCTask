using Microsoft.EntityFrameworkCore;
using NetPCTask.Data;
using NetPCTask.Models;

namespace NetPCTask.Services
{
    public class ContactService
    {
        private readonly AppDbContext _appDbContext;

        public ContactService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Contact GetContact(int id)
        {
            return _appDbContext.Contacts.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Contact> GetContacts()
        {
            return _appDbContext.Contacts.ToList();
        }

        public bool CreateContact(Contact contact)
        {
            _appDbContext.Add(contact);
            return Save();
        }

        public bool UpdateContact(Contact contact)
        {
            _appDbContext.Update(contact);
            return Save();
        }

        public bool DeleteContact(Contact contact)
        {
            _appDbContext.Remove(contact);
            return Save();
        }

        private bool Save()
        {
            var saved = _appDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool ContactExists(int id)
        {
            return _appDbContext.Contacts.Any(c => c.Id == id);
        }
    }
}
