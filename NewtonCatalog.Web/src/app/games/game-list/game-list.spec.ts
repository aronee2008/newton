import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of, throwError } from 'rxjs';
import { GameService } from '../game-service';
import { Game } from '../game.model';
import { GameList } from './game-list';

describe('GameList', () => {
  const games: Game[] = Array.from({ length: 25 }, (_, i) => ({
    id: i + 1,
    title: `Game ${i + 1}`,
    developer: 'Studio',
    platform: 'Pc',
    genre: 'Action',
    releaseDate: '2020-01-01',
    rating: 80,
    price: 9.99,
  }));

  async function create(getAll: () => unknown): Promise<ComponentFixture<GameList>> {
    TestBed.configureTestingModule({
      imports: [GameList],
      providers: [provideRouter([]), { provide: GameService, useValue: { getAll } }],
    });
    const fixture = TestBed.createComponent(GameList);
    await fixture.whenStable();
    return fixture;
  }

  it('shows the first page of ten games', async () => {
    const fixture = await create(() => of(games));

    const rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toBe(10);
    expect(rows[0].textContent).toContain('Game 1');
  });

  it('shows the remaining games on the last page', async () => {
    const fixture = await create(() => of(games));

    fixture.componentInstance.page.set(3);
    await fixture.whenStable();

    const rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toBe(5);
    expect(rows[0].textContent).toContain('Game 21');
  });

  it('shows an error when loading fails', async () => {
    const fixture = await create(() => throwError(() => new Error('down')));

    expect(fixture.nativeElement.querySelector('ngb-alert').textContent).toContain('Failed to load games.');
  });
});
