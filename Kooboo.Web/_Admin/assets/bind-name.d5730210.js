var B=Object.defineProperty,V=Object.defineProperties;var S=Object.getOwnPropertyDescriptors;var b=Object.getOwnPropertySymbols;var $=Object.prototype.hasOwnProperty,C=Object.prototype.propertyIsEnumerable;var g=(s,e,o)=>e in s?B(s,e,{enumerable:!0,configurable:!0,writable:!0,value:o}):s[e]=o,E=(s,e)=>{for(var o in e||(e={}))$.call(e,o)&&g(s,o,e[o]);if(b)for(var o of b(e))C.call(e,o)&&g(s,o,e[o]);return s},v=(s,e)=>V(s,S(e));var w=(s,e,o)=>new Promise((c,a)=>{var i=t=>{try{n(o.next(t))}catch(l){a(l)}},u=t=>{try{n(o.throw(t))}catch(l){a(l)}},n=t=>t.done?c(t.value):Promise.resolve(t.value).then(i,u);n((o=o.apply(s,e)).next())});import{g as F,o as I,w as K}from"./i18n.48bd28ac.js";import{_ as A}from"./container.d95ff706.js";import{d as R,E as q,x as d,g as M,cd as z,o as D,a as G,b as m,w as p,u as r,f,t as _,aH as H}from"./url.2e6a77c4.js";import{B as Q}from"./validate.efc4a5c0.js";import{r as T}from"./index.650df8b9.js";import{r as U}from"./index.4491c822.js";import{n as j}from"./guid.c1a40312.js";import{u as J}from"./use-first-input-focus.12142181.js";import{E as L,b as O}from"./main.ea42d807.js";import{E as P}from"./index.fc90f5ad.js";import{E as W,a as X}from"./index.6855d037.js";import{E as Y}from"./index.2bc50276.js";import{E as Z}from"./index.a439b087.js";import"./logo-transparent.1566007e.js";import"./light-switch.b4bd549c.js";import"./dark.ddf8665a.js";import"./plugin-vue_export-helper.41ffa612.js";import"./windi.a5b0b048.js";import"./index.d75a71d9.js";import"./index.aba77680.js";import"./replace-all.7cf5f327.js";import"./index.6d45a031.js";import"./index.31f26a0f.js";import"./index.60af85f4.js";import"./style.19d0c187.js";import"./toNumber.574be4f1.js";import"./_baseIsEqual.c0d7e77a.js";import"./index.50c16ae5.js";import"./preload-helper.13a99eb0.js";import"./event.776e7e11.js";import"./index.ff0264f3.js";import"./error.7e8331f1.js";import"./isNil.98bb3b88.js";import"./_baseClone.adbc92f5.js";import"./isEqual.f1ae9fb3.js";const oo={class:"h-full overflow-hidden"},eo={class:"flex items-center mb-16 text-m"},to={class:"text-444 mr-4"},Go=R({setup(s){const{t:e}=F();J();const o=q({userName:"",password:j(),verifyid:d("id")}),c={userName:Q},a=M(),i=z(),u=()=>w(this,null,function*(){yield a.value.validate();const t=yield(d("type")==="oauth"?U:T)(o);if(t.error){L.error(t.error);return}t.access_token&&(O().login(t.access_token),i.replace({name:"home"}))});return(n,t)=>{const l=P,x=W,k=X,N=Y,h=Z;return D(),G("div",oo,[m(h,null,{default:p(()=>[m(A,{title:r(e)("common.pickName"),onKeypress:I(u,["enter"])},{default:p(()=>[f("div",eo,[f("span",to,_(r(e)("common.haveKoobooAccount")),1),f("a",{class:"text-blue cursor-pointer","data-cy":"binding-account",onClick:t[0]||(t[0]=y=>r(i).push({name:"bind-account",query:v(E({},r(i).currentRoute.value.query),{id:r(o).verifyid,type:r(d)("type")})}))},_(r(e)("common.bindAccount")),1)]),m(k,{ref_key:"form",ref:a,"label-position":"top",model:r(o),rules:r(c),onSubmit:t[2]||(t[2]=K(()=>{},["prevent"]))},{default:p(()=>[m(x,{label:r(e)("common.username"),prop:"userName"},{default:p(()=>[m(l,{modelValue:r(o).userName,"onUpdate:modelValue":t[1]||(t[1]=y=>r(o).userName=y),"data-cy":"username"},null,8,["modelValue"])]),_:1},8,["label"])]),_:1},8,["model","rules"]),m(N,{class:"w-full text-l h-48px",round:"",type:"primary",size:"large","data-cy":"confirm",onClick:u},{default:p(()=>[H(_(r(e)("common.ok")),1)]),_:1})]),_:1},8,["title","onKeypress"])]),_:1})])}}});export{Go as default};