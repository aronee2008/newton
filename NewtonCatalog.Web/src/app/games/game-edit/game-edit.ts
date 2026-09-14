import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, input, numberAttribute, OnInit, signal } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NgbAlert } from '@ng-bootstrap/ng-bootstrap';
import { genreValues, platformValues } from '../api';
import { GameService } from '../game-service';

@Component({
  imports: [ReactiveFormsModule, RouterLink, NgbAlert],
  selector: 'app-game-edit',
  templateUrl: './game-edit.html',
})
export class GameEdit implements OnInit {
  private readonly gameService = inject(GameService);
  private readonly router = inject(Router);
  private readonly fb = inject(NonNullableFormBuilder);

  readonly id = input.required<number, string>({ transform: numberAttribute });
  readonly platforms = platformValues;
  readonly genres = genreValues;
  readonly error = signal<string | null>(null);

  readonly form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    developer: ['', [Validators.required, Validators.maxLength(100)]],
    platform: [platformValues[0]],
    genre: [genreValues[0]],
    releaseDate: ['', Validators.required],
    rating: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
    price: [0, [Validators.required, Validators.min(0)]],
  });

  ngOnInit() {
    this.gameService.getById(this.id()).subscribe({
      next: (game) => this.form.patchValue(game),
      error: (e: HttpErrorResponse) => this.error.set(e.status === 404 ? 'Game not found.' : 'Failed to load game.'),
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.gameService.update(this.id(), this.form.getRawValue()).subscribe({
      next: () => this.router.navigate(['/games']),
      error: () => this.error.set('Failed to save game.'),
    });
  }

  isInvalid(field: keyof typeof this.form.controls) {
    const control = this.form.controls[field];
    return control.invalid && control.touched;
  }
}
