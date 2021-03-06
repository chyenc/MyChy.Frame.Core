﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.UnitOfWork.Exceptions
{
    /// <summary>
    /// Base class for all <see cref="IUnitOfWork"/> related exceptions
    /// </summary>
    public class UnitOfWorkException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkException"/> class.
        /// </summary>
        public UnitOfWorkException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public UnitOfWorkException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkException"/> class with a specified error message and a 
        /// reference to the inner <see cref="Exception"/> that is the cause for this.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public UnitOfWorkException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
