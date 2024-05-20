using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetPCTask.Dto;
using NetPCTask.Models;
using NetPCTask.Services;

namespace NetPCTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactService _contactService;

        private readonly IMapper _mapper;

        public ContactController(ContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetContacts()
        {
            var contacts = _mapper.Map<List<ContactDto>>(_contactService.GetContacts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public IActionResult GetContact(int id)
        {
            if (!_contactService.ContactExists(id))
                return NotFound();

            var contact = _mapper.Map<ContactDto>(_contactService.GetContact(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(contact);
        }

        [HttpPost]
        public IActionResult AddContact([FromBody] ContactDto contact)
        {
            if (contact == null)
                return BadRequest(ModelState);

            var con = _contactService.GetContacts()
                .Where(c => c.Name.Trim().ToUpper() == contact.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (con != null)
            {
                ModelState.AddModelError("", "Contact already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactMap = _mapper.Map<Contact>(contact);

            if (!_contactService.CreateContact(contactMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to save");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{contactId}")]
        public IActionResult UpdateContact(int contactId, [FromBody] ContactDto contact)
        {
            if (contact == null)
                return BadRequest(ModelState);

            if (contact.Id != 0 && contact.Id != contactId)
                return BadRequest("Id in URL doesn't much id in request body");

            if (contact.Id == 0)
                contact.Id = contactId;

            if (!_contactService.ContactExists(contactId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var contactMap = _mapper.Map<Contact>(contact);

            if (!_contactService.UpdateContact(contactMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating the contact");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{contactId}")]
        public IActionResult DeleteContact(int contactId)
        {
            if (!_contactService.ContactExists(contactId))
            {
                return NotFound();
            }

            var contactToDelete = _contactService.GetContact(contactId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_contactService.DeleteContact(contactToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the contact");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
