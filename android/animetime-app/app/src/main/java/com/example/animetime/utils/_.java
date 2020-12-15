package com.example.animetime.utils;

public class _<T> {
    public T ref;

    public _(T ref) {
        this.ref = ref;
    }

    public void s(T t){
        this.ref = t;
    }
    public T g() {return ref;}
}
