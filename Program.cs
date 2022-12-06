// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using NAudio.Midi;
using System;

namespace Sample {
    class Program {
        private static readonly string[] NoteNames = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        static void Main(){
            int inputDevice = 0;
            if(MidiIn.NumberOfDevices == 0){
                Console.WriteLine("MIDIデバイスが見つかりませんでした");
                return ;
            }
            Console.WriteLine(MidiIn.NumberOfDevices + "件のMIDIデバイスが見つかりました");
            for(int i = 0; i < MidiIn.NumberOfDevices; i++){
                Console.WriteLine((i+1) + ": " + MidiIn.DeviceInfo(i).ProductName);    
            }

            Console.WriteLine("使用するデバイスの番号を入力してください");
            while(true){
                try{
                    var text = Console.ReadLine();
                    if(text != null){
                        inputDevice = int.Parse(text);
                        inputDevice -= 1;
                    }else{
                        Console.WriteLine("適切な数字を入力してください");
                        continue;
                    }
                    if(0 <= inputDevice && inputDevice < MidiIn.NumberOfDevices){
                        break;
                    }else{
                        Console.WriteLine("適切な数字を入力してください");
                        continue;
                    }
                }
                catch(FormatException)
                {
                    Console.WriteLine("適切な数字を入力してください");
                }
            }
            Console.WriteLine(MidiIn.DeviceInfo(inputDevice).ProductName + "を使用します");

            var midiIn = new MidiIn(inputDevice);
            #nullable enable
            midiIn.MessageReceived += midiIn_MessageReceived;
            #nullable disable
            midiIn.Start();
            while(true){
                var text = Console.ReadLine();
                if(text.Equals("stop")){
                    break;
                }
            }

            midiIn.Stop();
            midiIn.Dispose();

            return ;
        }

        static void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e){

            if(e.MidiEvent.CommandCode == MidiCommandCode.NoteOn){
                NoteEvent Event = e.MidiEvent as NoteEvent;
                if(Event != null){
                    int octave = Event.NoteNumber / 12 - 2;
                    Console.WriteLine(String.Format("{0}{1} vel:{2}",NoteNames[Event.NoteNumber%12], octave, Event.Velocity));
                }
            }            
        }
    }
}

