// Mirrors EmploymentStatus enum on the backend (Domain.Enums.EmploymentStatus)
export type EmploymentStatus = 'Active' | 'OnLeave' | 'Terminated';

export const EMPLOYMENT_STATUS_OPTIONS: EmploymentStatus[] = [
  'Active',
  'OnLeave',
  'Terminated'
];

// Matches EmployeeDto returned by GET endpoints.
export interface Employee {
  employeeId: number;
  firstName: string;
  lastName: string;
  email: string;
  department: string;
  employmentStatus: EmploymentStatus;
}

// Matches CreateEmployeeDto / UpdateEmployeeDto - same shape, so one type
// covers both POST and PUT requests. 
export interface EmployeeInput {
  firstName: string;
  lastName: string;
  email: string;
  department: string;
  employmentStatus: string;
}
