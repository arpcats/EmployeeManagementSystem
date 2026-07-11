import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EMPLOYMENT_STATUS_OPTIONS } from '../../core/models/employee.model';
import { EmployeeService } from '../../core/services/employee';

@Component({
  selector: 'app-employee-form',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.scss',
})
export class EmployeeForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly employeeService = inject(EmployeeService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly statusOptions = EMPLOYMENT_STATUS_OPTIONS;
  readonly isEditMode = signal(false);
  readonly saving = signal(false);
  readonly loading = signal(false);
  readonly serverError = signal<string | null>(null);

  private employeeId: number | null = null;

  // Reactive form with the same shape as EmployeeInput. Validators mirror
  // the backend's FluentValidation rules (required + max length + email
  // format) so obviously-invalid input never reaches the API - the backend
  // remains the source of truth and re-validates independently.
  readonly form = this.fb.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    department: ['', [Validators.required, Validators.maxLength(100)]],
    employmentStatus: ['', [Validators.required]]
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.employeeId = Number(idParam);
      this.isEditMode.set(true);
      this.loadEmployee(this.employeeId);
    }
  }

  private loadEmployee(id: number): void {
    this.loading.set(true);
    this.employeeService.getById(id).subscribe({
      next: employee => {
        this.form.patchValue(employee);
        this.loading.set(false);
      },
      error: () => {
        this.serverError.set('Unable to load this employee. It may have been removed.');
        this.loading.set(false);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    this.serverError.set(null);

    const payload = this.form.getRawValue();
    const request = this.isEditMode() && this.employeeId !== null
      ? this.employeeService.update(this.employeeId, payload)
      : this.employeeService.create(payload);

    request.subscribe({
      next: () => this.router.navigate(['/employees']),
      error: err => {
        this.saving.set(false);
        this.serverError.set(err?.error?.message ?? 'Something went wrong while saving. Please try again.');
      }
    });
  }

  // Convenience getters keep the template free of repeated `form.controls.x` lookups.
  get firstName() { return this.form.controls.firstName; }
  get lastName() { return this.form.controls.lastName; }
  get email() { return this.form.controls.email; }
  get department() { return this.form.controls.department; }
  get employmentStatus() { return this.form.controls.employmentStatus; }
}
