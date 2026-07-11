import { Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Employee } from '../../core/models/employee.model';
import { EmployeeService } from '../../core/services/employee';

@Component({
  selector: 'app-employee-list',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.scss',
})
export class EmployeeList implements OnInit {
  private readonly employeeService = inject(EmployeeService);
  private readonly destroyRef = inject(DestroyRef);

  readonly employees = signal<Employee[]>([]);
  readonly loading = signal(true);
  readonly errorMessage = signal<string | null>(null);

  // Reactive form control (not a full form) since search is a single field -
  readonly searchControl = new FormControl('', { nonNullable: true });

  ngOnInit(): void {
    this.loadEmployees();

    this.searchControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntilDestroyed(this.destroyRef))
      .subscribe(term => this.loadEmployees(term));
  }

  private loadEmployees(search?: string): void {
    this.loading.set(true);
    this.errorMessage.set(null);

    this.employeeService.getAll(search).subscribe({
      next: employees => {
        this.employees.set(employees);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Unable to load employees. Please make sure the API is running.');
        this.loading.set(false);
      }
    });
  }
}
