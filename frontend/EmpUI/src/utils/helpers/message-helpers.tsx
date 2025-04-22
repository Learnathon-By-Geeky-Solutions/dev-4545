
export const validationMessage = (label: string, rule?: 'number' | 'email'): string => {
  switch (rule) {
    case 'number': {
      return `The ${label} field must be a number!`;
    }
    case 'email': {
      return `The ${label} field must be a valid email address!`;
    }
    default: {
      return `The ${label} field is required!`;
    }
  }
};