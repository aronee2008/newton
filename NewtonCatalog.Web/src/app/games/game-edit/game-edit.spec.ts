import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { of } from 'rxjs';
import { GameService } from '../game-service';
import { Game } from '../game.model';
import { GameEdit } from './game-edit';

describe('GameEdit', () => {
  let fixture: ComponentFixture<GameEdit>;
  let component: GameEdit;
  let gameService: { getById: ReturnType<typeof vi.fn>; update: ReturnType<typeof vi.fn> };
  let router: Router;

  const hades: Game = {
    id: 1,
    title: 'Hades',
    developer: 'Supergiant Games',
    platform: 'Pc',
    genre: 'Action',
    releaseDate: '2020-09-17',
    rating: 93,
    price: 24.99,
  };

  beforeEach(async () => {
    gameService = { getById: vi.fn(() => of(hades)), update: vi.fn(() => of(undefined)) };
    TestBed.configureTestingModule({
      imports: [GameEdit],
      providers: [provideRouter([]), { provide: GameService, useValue: gameService }],
    });
    router = TestBed.inject(Router);
    vi.spyOn(router, 'navigate').mockResolvedValue(true);

    fixture = TestBed.createComponent(GameEdit);
    fixture.componentRef.setInput('id', '1');
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('loads the game into the form', () => {
    expect(gameService.getById).toHaveBeenCalledWith(1);
    expect(component.form.getRawValue()).toEqual({
      title: 'Hades',
      developer: 'Supergiant Games',
      platform: 'Pc',
      genre: 'Action',
      releaseDate: '2020-09-17',
      rating: 93,
      price: 24.99,
    });
  });

  it('does not save an invalid form', () => {
    component.form.patchValue({ title: '', rating: 150 });

    component.save();

    expect(component.form.controls.title.invalid).toBe(true);
    expect(component.form.controls.rating.invalid).toBe(true);
    expect(gameService.update).not.toHaveBeenCalled();
  });

  it('saves a valid form and returns to the list', () => {
    component.form.patchValue({ title: 'Hades II', rating: 90 });

    component.save();

    expect(gameService.update).toHaveBeenCalledWith(1, expect.objectContaining({ title: 'Hades II', rating: 90 }));
    expect(router.navigate).toHaveBeenCalledWith(['/games']);
  });
});
