var g=(a,o,t)=>new Promise((r,s)=>{var u=e=>{try{m(t.next(e))}catch(n){s(n)}},f=e=>{try{m(t.throw(e))}catch(n){s(n)}},m=e=>e.done?r(e.value):Promise.resolve(e.value).then(u,f);m((t=t.apply(a,o)).next())});import{a as i}from"./replace-all.7cf5f327.js";import{d as c}from"./main.ea42d807.js";import{d as l}from"./i18n.48bd28ac.js";import{g as T}from"./url.2e6a77c4.js";l.global.t;const d=a=>c.get(i("CommerceTag/list"),{type:a}),p=(a,o)=>c.post(i("CommerceTag/Delete"),{type:a,name:o});function x(a){const o=T([]);function t(){return g(this,null,function*(){o.value=yield d(a)})}t();function r(s){return g(this,null,function*(){yield p(a,s),t()})}return{tags:o,loadTags:t,removeTag:r}}export{x as u};