import { provideRouter } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { EmployeeList } from './employee-list';
import { EmployeeService } from '../../core/services/employee';
import { Employee } from '../../core/models/employee.model';

describe('EmployeeList', () => {
  let component: EmployeeList;
  let fixture: ComponentFixture<EmployeeList>;
  let employeeServiceSpy: jasmine.SpyObj<EmployeeService>;

  const mockEmployees: Employee[] = [
    { employeeId: 1, firstName: 'John', lastName: 'Doe', email: 'john.doe@testmail.com', department: 'HR', employmentStatus: 'Active' },
    { employeeId: 2, firstName: 'Tony', lastName: 'Payumo', email: 'tony.payumo@testmail.com', department: 'IT', employmentStatus: 'OnLeave' }
  ];

  beforeEach(async () => {
    employeeServiceSpy = jasmine.createSpyObj('EmployeeService', ['getAll']);
    employeeServiceSpy.getAll.and.returnValue(of(mockEmployees));

    await TestBed.configureTestingModule({
      imports: [EmployeeList],
      providers: [
        provideRouter([]),
        { provide: EmployeeService, useValue: employeeServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EmployeeList);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('loads employees on init and stops the loading state', () => {
    fixture.detectChanges();

    expect(employeeServiceSpy.getAll).toHaveBeenCalledWith(undefined);
    expect(component.employees()).toEqual(mockEmployees);
    expect(component.loading()).toBeFalse();
  });

  it('sets an error message when the API call fails', () => {
    employeeServiceSpy.getAll.and.returnValue(throwError(() => new Error('network error')));

    fixture.detectChanges();

    expect(component.errorMessage()).toContain('Unable to load employees');
    expect(component.loading()).toBeFalse();
  });
});
