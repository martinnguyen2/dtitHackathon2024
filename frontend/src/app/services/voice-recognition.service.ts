import { EventEmitter, Injectable, Output } from '@angular/core';
declare let webkitSpeechRecognition: any;

@Injectable({
    providedIn: 'root',
})
export class VoiceRecognitionService {
    @Output() textChanged: EventEmitter<string> = new EventEmitter<string>();
    recognition = typeof webkitSpeechRecognition !== "undefined" ? new webkitSpeechRecognition() : undefined;
    isStoppedSpeechRecog = false;
    public text = '';
    tempWords: any;
    transcriptArr: string[] = [];
    confidenceArr: string[] = [];
    isStarted = false; // << this Flag to check if the user stop the service
    isStoppedAutomatically = true; // << this Flag to check if the service stopped automaticically.

    constructor() {}

    init() {
        this.recognition.continuous = true;
        this.recognition.interimResults = true;
        this.recognition.lang = 'en-US';

        this.recognition.addEventListener('result', (e: any) => {
            const transcript = Array.from(e.results)
                .map((result: any) => result[0])
                .map((result) => result.transcript)
                .join('');
            this.transcriptArr.push(transcript);
            this.tempWords = transcript;
            console.log(this.transcriptArr);

            const confidence = Array.from(e.results)
                .map((result: any) => result[0])
                .map((result) => result.confidence)
                .join('');
            this.confidenceArr.push(confidence);
            console.log(this.confidenceArr);

            this.textChanged.emit(this.text + this.transcriptArr[this.transcriptArr.length - 1]);
        });

        this.recognition.addEventListener('end', (condition: any) => {
            this.wordConcat();
            if (this.isStoppedAutomatically) {
                this.recognition.stop();
                console.log('stopped automatically!!');
                this.recognition.start();
                console.log('started automatically!!');
                this.isStoppedAutomatically = true;
            }
        });
    }

    start() {
        if (!this.isStarted) {
            this.recognition.start();
            this.isStarted = true;
            console.log('Speech recognition started');
        }
        return true;
    }
    stop() {
        if (this.isStarted) {
            this.isStoppedAutomatically = false;
            this.wordConcat();
            this.recognition.stop();
            this.isStarted = false;
            console.log('End speech recognition by user');
        }

        this.tempWords = '';
        this.transcriptArr = [];
        this.confidenceArr = [];
        return false;
    }

    wordConcat() {
        this.text = this.text + ' ' + this.tempWords;
        this.tempWords = '';

        this.textChanged.emit(this.text);
    }
}