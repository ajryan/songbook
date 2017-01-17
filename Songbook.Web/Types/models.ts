﻿/* tslint:disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v8.5.6214.27522 (NJsonSchema v7.3.6214.20986) (http://NSwag.org)
// </auto-generated>
//----------------------

namespace Songbook.Core {

export interface Song {
    Id?: string;
    Name?: string;
    Lyrics?: Lyric[];
    Progressions?: Progression[];
}

export interface Lyric {
    Name?: string;
    Words?: string;
}

export interface Progression {
    TimeSignature?: TimeSignature;
    TempoBpm: number;
    Chords?: ChordEvent[];
}

export interface TimeSignature {
    Beats: number;
    Note: number;
}

export interface ChordEvent {
    Chord?: Chord;
    StartBeat: number;
    DurationBeats: number;
}

export interface Chord {
    Note?: Note;
    Key?: Key;
    IsSeventh: boolean;
    BaseNote?: Note;
    Augmentations?: Augmentation[];
}

export interface Note {
    Pitch: Pitch;
    Accidental?: Accidental;
}

export enum Pitch {
    C = 0, 
    D = 1, 
    E = 2, 
    F = 3, 
    G = 4, 
    A = 5, 
    B = 6, 
}

export enum Accidental {
    Flat = 0, 
    Sharp = 1, 
}

export enum Key {
    Major = 0, 
    Minor = 1, 
}

export interface Augmentation {
    Accidental: Accidental;
    Interval: number;
}

}