using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Domain.Entities;

public sealed class Customer
{
    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public string Email { get; private set; } = default!;

    public string PhoneNumber { get; private set; } = default!;

    public string IdentityNumber { get; private set; } = default!;

    public string? PhotoUrl { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private Customer()
    {
    }

    public Customer(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string identityNumber)
    {
        Id = Guid.CreateVersion7();

        FirstName = firstName;

        LastName = lastName;

        Email = email;

        PhoneNumber = phoneNumber;

        IdentityNumber = identityNumber;

        IsActive = true;

        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateEmail(string email)
    {
        Email = email;
        UpdatedAt = DateTime.UtcNow;

    }
    public void UpdatePhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdatePhoto(string photoUrl)
    {
        PhotoUrl = photoUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
    string firstName,
    string lastName,
    string email,
    string phoneNumber,
    string? photoUrl)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        PhotoUrl = photoUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}

