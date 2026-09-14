import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbAlert, NgbPagination } from '@ng-bootstrap/ng-bootstrap';
import { GameService } from '../game-service';
import { Game } from '../game.model';

@Component({
  imports: [RouterLink, NgbPagination, NgbAlert, DatePipe, CurrencyPipe],
  selector: 'app-game-list',
  templateUrl: './game-list.html',
})
export class GameList implements OnInit {
  private readonly gameService = inject(GameService);

  readonly games = signal<Game[]>([]);
  readonly error = signal<string | null>(null);
  readonly page = signal(1);
  readonly pageSize = 10;

  readonly pageOfGames = computed(() => {
    const start = (this.page() - 1) * this.pageSize;
    return this.games().slice(start, start + this.pageSize);
  });

  ngOnInit() {
    this.gameService.getAll().subscribe({
      next: (games) => this.games.set(games),
      error: () => this.error.set('Failed to load games.'),
    });
  }
}
