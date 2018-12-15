namespace Jacobi.Vst.Samples.MidiNoteMapper
{
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;


    class ChordTable
    {
        private List<Chord> _Chords;
        public List<Chord> Chords {
            get { return _Chords; }
            set { _Chords = value; }
        }
        public ChordTable(List<Chord>Chords)
        {
            _Chords = Chords;
        }
    }

    class Chord: IEnumerable<NoteList>
    {
        private List<Note> _Chord;
        public List<Note> NoteList {
            get {
                return _Chord;}
            set {}
        }
        public string ChordName { get; set; }

        public IEnumerator<NoteList> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
        //public void Add(Note Value)
        //{
        //    _Chord.Add(Value);
        //}
        public Chord(string Name, List<int> Values)
        {
            _Chord = new List<Note>();
            this.Add(Name, Values);
        }

        public Chord(List<int> Values)
        {
            _Chord = new List<Note>();
            this.Add(Values);
        }

        public void Add(string Name, List<int> Values)
        {
            ChordName = Name;
            this.Add(Values);
        }
        public void Add(List<int> Values)
        {
            foreach (int NoteValue in Values)
            {
                Note Chordnote = new Note(NoteValue);
                _Chord.Add(Chordnote);
            }
        }
    }
    class Note
    {
        private int _Notenumber;
        public int Notenumber {
            get {return _Notenumber;}
            set { _Notenumber = value; }
        }
        public Note(int Value)
        {
            _Notenumber = Value;
        }
        
        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
    /// <summary>
    /// Implements the incoming Midi event handling for the plugin.
    /// </summary>
    class MidiProcessor : IVstMidiProcessor
    {
        private Plugin _plugin;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="plugin">Must not be null.</param>

        private readonly ChordTable Chords;
           

        public MidiProcessor(Plugin plugin)
        {
            _plugin = plugin;
            Events = new VstEventCollection();
            NoteOnEvents = new Queue<byte>();

            List<Chord> DefaultChords = new List<Chord>();
            //Major Chords
            DefaultChords.Add(new Chord("",new List<int> { 0, 4, 7 } ));
            DefaultChords.Add(new Chord("6", new List<int> { 0, 4, 7, 9 }));
            DefaultChords.Add(new Chord(" add9", new List<int> { 0, 4, 7, 14 }));
            DefaultChords.Add(new Chord("6/9", new List<int> { 0, 4, 7, 9, 14 }));
            DefaultChords.Add(new Chord(" sus2", new List<int> { 0, 2, 7}));
            DefaultChords.Add(new Chord(" sus4", new List<int> { 0, 5, 7 }));
            DefaultChords.Add(new Chord(" maj7", new List<int> { 0, 4, 7, 11 }));
            DefaultChords.Add(new Chord(" maj7/#5", new List<int> { 0, 4, 8, 11 }));
            DefaultChords.Add(new Chord(" maj7/9", new List<int> { 0, 4, 7, 11, 14 }));
            DefaultChords.Add(new Chord(" maj7/#11", new List<int> { 0, 4, 11, 18 }));
            DefaultChords.Add(new Chord(" maj7/13", new List<int> { 0, 4, 7, 11, 21 }));
            DefaultChords.Add(new Chord(" maj7/9/13", new List<int> { 0, 4, 7, 11, 14, 21 }));
            //Minor Chords
            DefaultChords.Add(new Chord("m", new List<int> { 0, 3, 7 }));
            DefaultChords.Add(new Chord("m6", new List<int> { 0, 3, 7, 9 }));
            DefaultChords.Add(new Chord("m6/9", new List<int> { 0, 3, 7, 9, 14 }));
            DefaultChords.Add(new Chord("m7", new List<int> { 0, 3, 7, 10 }));
            DefaultChords.Add(new Chord("m7/b5", new List<int> { 0, 3, 6, 10 }));
            DefaultChords.Add(new Chord("m7/9", new List<int> { 0, 3, 7, 10, 14 }));
            DefaultChords.Add(new Chord("m maj7", new List<int> { 0, 3, 7, 11 }));
            DefaultChords.Add(new Chord("m maj7/9", new List<int> { 0, 3, 7, 11, 14 }));
            DefaultChords.Add(new Chord("m add9", new List<int> { 0, 3, 7, 14 }));
            DefaultChords.Add(new Chord("m7/11", new List<int> { 0, 3, 7, 10, 17 }));
            DefaultChords.Add(new Chord("m7/9/11", new List<int> { 0, 3, 7, 10, 14, 17 }));
            DefaultChords.Add(new Chord("m add11", new List<int> { 0, 3, 7, 17 }));
            //Dominant Sept Chords
            DefaultChords.Add(new Chord("7", new List<int> { 0, 4, 7, 10 }));
            DefaultChords.Add(new Chord("7/sus4", new List<int> { 0, 5, 7, 10 }));
            DefaultChords.Add(new Chord("7/9", new List<int> { 0, 4, 7, 10, 14 }));
            DefaultChords.Add(new Chord("7/9/13", new List<int> { 0, 4, 7, 10, 14, 21 }));
            DefaultChords.Add(new Chord("7/9/#11", new List<int> { 0, 4, 10, 14, 18 }));
            DefaultChords.Add(new Chord("7/9/b13", new List<int> { 0, 4, 10, 14, 20 }));
            DefaultChords.Add(new Chord("7/b9", new List<int> { 0, 4, 7, 10, 13 }));
            DefaultChords.Add(new Chord("7/b9/#11", new List<int> { 0, 4, 10, 13, 18 }));
            DefaultChords.Add(new Chord("7/b9/13", new List<int> { 0, 4, 7, 10, 13, 21 }));
            DefaultChords.Add(new Chord("7/b9/b13", new List<int> { 0, 4, 10, 13, 20 }));
            DefaultChords.Add(new Chord("7/#9", new List<int> { 0, 4, 7, 10, 15 }));
            DefaultChords.Add(new Chord("7/#9/#11", new List<int> { 0, 4, 10, 15, 18 }));
            DefaultChords.Add(new Chord("7/#9/13", new List<int> { 0, 4, 7, 10, 13, 21 }));
            DefaultChords.Add(new Chord("7/#9/b13", new List<int> { 0, 4, 10, 15, 20 }));
            DefaultChords.Add(new Chord("7/#11", new List<int> { 0, 4, 10, 18 }));
            DefaultChords.Add(new Chord("7/13", new List<int> { 0, 4, 7, 10, 21 }));
            DefaultChords.Add(new Chord("7/13/sus4", new List<int> { 0, 5, 7, 10, 21 }));
            DefaultChords.Add(new Chord("7/b13/sus4", new List<int> { 0, 4, 10, 20 }));
            //Diminished Chords
            DefaultChords.Add(new Chord("o7", new List<int> { 0, 3, 6, 9 }));
            DefaultChords.Add(new Chord("+", new List<int> { 0, 4, 8 }));

            Chords = new ChordTable(DefaultChords);

        }

        /// <summary>
        /// Gets the midi events that should be processed in the current cycle.
        /// </summary>
        public VstEventCollection Events { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating wether non-mapped midi events should be passed to the output.
        /// </summary>
        public bool MidiThru { get; set; }

        /// <summary>
        /// The raw note on note numbers.
        /// </summary>
        public Queue<byte> NoteOnEvents { get; private set; }

        #region IVstMidiProcessor Members

        public int ChannelCount
        {
            get { return _plugin.ChannelCount; }
        }

       

        public byte KeySplit = 60;
            private int State = 0;
            bool Pedal = false;
       
        public byte BassNote = 0;

        /// ///////////////////

        public event EventHandler Playing;

        public void OnPlaying(EventArgs e)
        {
            EventHandler handler = Playing;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public class PlayingArgs : EventArgs
        {
            public string PlayingChordText;
        }

        private List<VstMidiEvent> _PlayingNotes = new List<VstMidiEvent>();

        //private Chord[] ChordTable;

        //Rapid Spagetti
        
        public void Process(VstEventCollection events)
        {
            

            foreach (VstEvent evnt in events)
            {
                
                if (evnt.EventType != VstEventTypes.MidiEvent) continue;

                VstMidiEvent[] midiEvent = new VstMidiEvent[24];
                VstMidiEvent initmidiEvent = (VstMidiEvent)evnt;
                midiEvent[0] = initmidiEvent;
                VstMidiEvent mappedEvent = null;
                //&& ((midiEvent[0].Data[2] & 0xF0) == 0x7F)
                if (((midiEvent[0].Data[0] & 0xF0) == 0xB0) && (midiEvent[0].Data[2] == 127)) { Pedal = true; }
                if (((midiEvent[0].Data[0] & 0xF0) == 0xB0) && (midiEvent[0].Data[2] == 0)) { Pedal = false; midiEvent[0].Data[0] = 0x80; midiEvent[0].Data[1] = (byte)(KeySplit - 1); }

                if (((midiEvent[0].Data[0] & 0xF0) == 0x90) && (midiEvent[0].Data[1] < KeySplit)) // Note On below KeySplit
                                                                                                  //((midiEvent.Data[0] & 0xF0) == 0x80 ||Note Off
                {
                    State = 1;
                    BassNote = midiEvent[0].Data[1];
                }
                if ((midiEvent[0].Data[0] & 0xF0) == 0x90 && (midiEvent[0].Data[1] >= KeySplit) && State == 1) // Note On above KeySplit + State 1 Already Bass Note Selected
                {
                    //if ((midiEvent.Data[1] < KeySplit)
                    //Play Cord from Table
                    State = 2;
                    PlayingArgs Args = new PlayingArgs();
                    Args.PlayingChordText = Chords.Chords[midiEvent[0].Data[1] - (byte)KeySplit].ChordName;

                    OnPlaying(Args);
                    //                  
                    EventArgs e = new EventArgs();
                     //e = Chords.Chords[midiEvent[0].Data[1] - KeySplit].ChordName;
                    //OnPlaying(e);

                    //public event System.EventHandler Chord_Playing;
                    //MidiProcessor



                   Note ThisNote = new Note(0);
                    for (int i = 0; i < Chords.Chords[midiEvent[0].Data[1] - (byte)KeySplit].NoteList.Count; i++)
                    {
                        
                        ThisNote = Chords.Chords[midiEvent[0].Data[1] - (byte)KeySplit].NoteList[i];
                        //if (i == 0)
                        //{
                        //lock (((ICollection)NoteOnEvents).SyncRoot)
                        //{
                        
                        //}
                        //} else
                        //{
                        //lock (((ICollection)NoteOnEvents).SyncRoot)
                        //{
                        //NoteOnEvents.Enqueue((byte)(ThisNote.Notenumber + BassNote));
                        //}

                        // Might be optimizeable as long as noTe on the next Notes do not need the redundant status
                        // according to https://www.csie.ntu.edu.tw/~r92092/ref/midi/midi_messages.html

                        //byte ChordNote = (byte)(Chords.Chords[midiEvent[0].Data[1] - (byte)KeySplit].NoteList[i].Notenumber + BassNote); //suppose it takes the least significant byte
                        //lock (((ICollection)NoteOnEvents).SyncRoot)
                        //{
                        //    NoteOnEvents.Enqueue(ChordNote);
                        //} 
                        // do this for now
                        //}
                        byte[] midiData = new byte[4];
                        midiData[0] = midiEvent[0].Data[0]; //should be Note on here
                        midiData[1] = (byte)(ThisNote.Notenumber + BassNote + 12);  //Notevalue will be set in the loop 
                        midiData[2] = midiEvent[0].Data[2]; //Vel
                        mappedEvent = new VstMidiEvent(midiEvent[0].DeltaFrames,
                                midiEvent[0].NoteLength,
                               midiEvent[0].NoteOffset,
                               midiData,
                               midiEvent[0].Detune,
                               midiEvent[0].NoteOffVelocity);
                        lock (((ICollection)NoteOnEvents).SyncRoot)
                        {
                            Events.Add(mappedEvent);
                        }
                        _PlayingNotes.Add(mappedEvent);
                        
                        mappedEvent = null;
                        midiData = null;
                    }
                    ThisNote = null;
                }

                if ((midiEvent[0].Data[0] & 0xF0) == 0x80) // Note Off
                {
                    if (midiEvent[0].Data[1] < KeySplit) { State = 0; } else { State = State - 1; }

                    if (Pedal == false)
                    {
                        byte[] midiData = new byte[4];
                        midiData[0] = midiEvent[0].Data[0]; //is Note off here
                        midiData[1] = 0;                 // 
                        midiData[2] = midiEvent[0].Data[2]; //
                        foreach (VstMidiEvent Note in _PlayingNotes)
                        {
                            Note.Data[0] = 0x80;
                            Events.Add(Note);
                        }
                        _PlayingNotes.Clear();
                    } 
                    //foreach (byte PlayingNote in NoteOnEvents)
                    //{
                    //    lock (((ICollection)NoteOnEvents).SyncRoot)
                    //    {
                    //        Events.Add(evnt);
                    //    }
                    //}
                }
                //must get rid of the NoteMap if (cleanup)
                //{

                //if (_plugin.NoteMap.Contains(midiEvent.Data[1])) 
                //{
                //    byte[] midiData = new byte[4];
                //    midiData[0] = midiEvent.Data[0];
                //    midiData[1] = _plugin.NoteMap[midiEvent.Data[1]].OutputNoteNumber;
                //    midiData[2] = midiEvent.Data[2];

                //    mappedEvent = new VstMidiEvent(midiEvent.DeltaFrames, 
                //        midiEvent.NoteLength, 
                //        midiEvent.NoteOffset, 
                //        midiData, 
                //        midiEvent.Detune, 
                //        midiEvent.NoteOffVelocity);

                //    Events.Add(mappedEvent);

                //    // add raw note-on note numbers to the queue
                //    if((midiEvent.Data[0] & 0xF0) == 0x90)
                //    {
                //        lock (((ICollection)NoteOnEvents).SyncRoot)
                //        {
                //            NoteOnEvents.Enqueue(midiEvent.Data[1]);
                //        }
                //    }
                //}
                else if (MidiThru)
                {
                    // add original event
                    Events.Add(evnt);
                }
            }
        }

       
        #endregion
    }
}
