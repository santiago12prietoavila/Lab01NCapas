using System;
using System.Collections.Generic;
using DAL;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Exceptions;

public class CustomerExceptions : Exception
{
    // you can more static methods here to throw other customer-related exceptions
    private CustomerExceptions (string message) : base (message)
    { 
        //optional: Add constructor logic for logging or custom error handling
    }

    public static void ThrowCustomerAlreadyExistsException(string FirstName, string LastName)
    {
        throw new CustomerExceptions($"A client with the name already exists{FirstName}{LastName}.");
    }

    public static void ThrowInvalidCustomerDataException(string message)
    {
        throw new CustomerExceptions (message);
    }

    public static void ThrowInvalidCustomerIdException(int customerId)
    {
        throw new CustomerExceptions($"No customer found with ID {customerId}.");
    }

}
