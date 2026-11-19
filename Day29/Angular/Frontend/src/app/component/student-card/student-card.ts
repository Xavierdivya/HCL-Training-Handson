import { Component, input } from '@angular/core';
import { StudentRecord } from '../../services/student';
import { CommonModule } from '@angular/common';
import { EmailHoverDirective } from '../../directives/email-hover.directive';

@Component({
  selector: 'app-student-card',
  imports: [CommonModule, EmailHoverDirective],
  templateUrl: './student-card.html',
  styleUrl: './student-card.css',
})
export class StudentCard {

  student = input.required<StudentRecord>();
  today = new Date();
}