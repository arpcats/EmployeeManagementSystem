import { ActivatedRoute, convertToParamMap, provideRouter, Router } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { EmployeeForm } from './employee-form';
import { EmployeeService } from '../../core/services/employee';
import { Employee } from '../../core/models/employee.model';

describe('EmployeeForm', () => {
  let component: EmployeeForm;
  let fixture: ComponentFixture<EmployeeForm>;
  let employeeServiceSpy: jasmine.SpyObj<EmployeeService>;

  async function setup(paramMap: Record<string, string> = {}) {
    employeeServiceSpy = jasmine.createSpyObj('EmployeeService', ['getById', 'create', 'update']);

    await TestBed.configureTestingModule({
      imports: [EmployeeForm],
      providers: [
        provideRouter([]),
        { provide: EmployeeService, useValue: employeeServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: convertToParamMap(paramMap) } }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EmployeeForm);
    component = fixture.componentInstance;
  }

  it('should create in "add" mode when no id param is present', async () => {
    await setup();
    fixture.detectChanges();

    expect(component.isEditMode()).toBeFalse();
    expect(employeeServiceSpy.getById).not.toHaveBeenCalled();
  });

  it('marks the form invalid and does not call create() when required fields are empty', async () => {
    await setup();
    fixture.detectChanges();

    component.onSubmit();

    expect(component.form.invalid).toBeTrue();
    expect(employeeServiceSpy.create).not.toHaveBeenCalled();
  });

  it('calls create() with the form value and navigates away on success', async () => {
    await setup();
    fixture.detectChanges();

    const router = TestBed.inject(Router);
    spyOn(router, 'navigate');

    employeeServiceSpy.create.and.returnValue(of({
      employeeId: 1,
      firstName: 'Jose',
      lastName: 'Rizal',
      email: 'jose.rizal@testmail.com',
      department: 'Finance',
      employmentStatus: 'OnLeave'
    } as Employee));

    component.form.setValue({
      firstName: 'Jose',
      lastName: 'Rizal',
      email: 'jose.rizal@testmail.com',
      department: 'Finance',
      employmentStatus: 'OnLeave'
    });

    component.onSubmit();

    expect(employeeServiceSpy.create).toHaveBeenCalledWith(component.form.getRawValue());
    expect(router.navigate).toHaveBeenCalledWith(['/employees']);
  });

  it('loads the existing employee and calls update() in edit mode', async () => {
    const existing: Employee = {
      employeeId: 3,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@testmail.com',
      department: 'Marketing',
      employmentStatus: 'Active'
    };

    await setup({ id: '3' });
    employeeServiceSpy.getById.and.returnValue(of(existing));
    employeeServiceSpy.update.and.returnValue(of(existing));

    fixture.detectChanges();

    expect(component.isEditMode()).toBeTrue();
    expect(employeeServiceSpy.getById).toHaveBeenCalledWith(3);
    expect(component.form.value.firstName).toBe('John');

    component.onSubmit();

    expect(employeeServiceSpy.update).toHaveBeenCalledWith(3, component.form.getRawValue());
  });
});
