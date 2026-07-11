import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../../../environments/environment';
import { Employee, EmployeeInput } from '../models/employee.model';
import { EmployeeService } from './employee';

describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;
  const baseUrl = `${environment.apiBaseUrl}/employees`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EmployeeService, provideHttpClient(), provideHttpClientTesting()]
    });

    service = TestBed.inject(EmployeeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    // Fail the test if a spec forgot to expect a request, or if the
    // component under test fired an extra one we didn't account for.
    httpMock.verify();
  });

  it('getAll() with no search term requests the base employees URL', () => {
    const mockEmployees: Employee[] = [
      { employeeId: 1, firstName: 'John', lastName: 'Doe', email: 'john.doe@testmail.com', department: 'HR', employmentStatus: 'Active' }
    ];

    service.getAll().subscribe(employees => {
      expect(employees).toEqual(mockEmployees);
    });

    const req = httpMock.expectOne(baseUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockEmployees);
  });

  it('getAll(search) appends the search query param', () => {
    service.getAll('john').subscribe();

    const req = httpMock.expectOne(request => request.url === baseUrl && request.params.get('search') === 'john');
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('create() posts the employee payload to the base URL', () => {
    const input: EmployeeInput = {
      firstName: 'Tony',
      lastName: 'Payumo',
      email: 'tony.payumo@testmail.com',
      department: 'IT',
      employmentStatus: 'Active'
    };
    const created: Employee = { employeeId: 2, ...input, employmentStatus: 'Active' };

    service.create(input).subscribe(employee => {
      expect(employee).toEqual(created);
    });

    const req = httpMock.expectOne(baseUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(input);
    req.flush(created);
  });
});
