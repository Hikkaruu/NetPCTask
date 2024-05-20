// method checks password complexity
export function checkPasswordComplexity(password) {
    const minLength = 8;
    const lowercaseRegex = /[a-z]/;
    const uppercaseRegex = /[A-Z]/;
    const digitRegex = /[0-9]/;
    const specialCharRegex = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
  
    if (password.length < minLength) 
        return 'Password must be at least 8 characters long.';
    
    if (!lowercaseRegex.test(password)) 
        return 'Password must contain at least one lowercase letter.';
    
    if (!uppercaseRegex.test(password)) 
        return 'Password must contain at least one uppercase letter.';
    
    if (!digitRegex.test(password)) 
        return 'Password must contain at least one digit.';

    if (!specialCharRegex.test(password)) 
        return 'Password must contain at least one special character.';
    
    return 'Password is complex enough.';
  };