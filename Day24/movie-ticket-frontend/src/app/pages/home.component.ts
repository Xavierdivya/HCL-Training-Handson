import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="hero">
      <div class="hero-copy">
        <p class="eyebrow">YOUR SEAT. YOUR SHOW.</p>
        <h1>Movies are better<br><span>on the big screen.</span></h1>
        <p>Discover movies, explore showtimes and book your seats in a few simple steps.</p>
        <a class="btn btn-primary btn-large" routerLink="/movies">Explore movies</a>
      </div>
      <div class="hero-art"><div class="ticket">CINE<br><b>BOOK</b><small>ADMIT ONE</small></div></div>
    </section>
    <section class="feature-row">
      <article><b>Discover</b><span>Browse movies and showtimes.</span></article>
      <article><b>Choose</b><span>Select a show and your seats.</span></article>
      <article><b>Book</b><span>Keep your booking details handy.</span></article>
    </section>
  `
})
export class HomeComponent {}
