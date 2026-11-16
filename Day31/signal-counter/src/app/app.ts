
import { Component, signal, computed, effect } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  // 1. Signal: Stores the counter value
  count = signal(0);

  // 2. Computed: Derives the square of the counter
  square = computed(() => this.count() * this.count());

  // Computed: Checks whether the number is even or odd
  status = computed(() =>
    this.count() % 2 === 0 ? 'Even' : 'Odd'
  );

  constructor() {

    // 3. Effect: Runs whenever count changes
    effect(() => {
      console.log('Counter updated:', this.count());
    });

  }

  increment() {
    this.count.update(value => value + 1);
  }

  decrement() {
    this.count.update(value => value - 1);
  }

  reset() {
    this.count.set(0);
  }
}