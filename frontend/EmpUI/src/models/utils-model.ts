import React from 'react';

export interface AppError {
  data: {
    error?: string;
    error_description?: string;
  }
}

export interface MenuItem {
  key: string;
  label: React.ReactNode;
  icon?: React.ReactNode;
  children?: MenuItem[];
}

export interface QueryParams {
  [key: string]: any;
}