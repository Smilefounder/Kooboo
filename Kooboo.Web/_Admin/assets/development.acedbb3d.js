var x=Object.defineProperty,y=Object.defineProperties;var b=Object.getOwnPropertyDescriptors;var u=Object.getOwnPropertySymbols;var I=Object.prototype.hasOwnProperty,E=Object.prototype.propertyIsEnumerable;var d=(r,o,t)=>o in r?x(r,o,{enumerable:!0,configurable:!0,writable:!0,value:t}):r[o]=t,_=(r,o)=>{for(var t in o||(o={}))I.call(o,t)&&d(r,t,o[t]);if(u)for(var t of u(o))E.call(o,t)&&d(r,t,o[t]);return r},g=(r,o)=>y(r,b(o));import{_ as $,a as B}from"./kmail-button.36b5d9a0.js";import{_ as M,a as j,D}from"./code-conflict.3f6cebc6.js";import{b as V,u as R,c as A,r as L,E as O,i as P}from"./main.ea42d807.js";import{d as T,M as q,g as z,i as G,b2 as v,N as H,o as p,j as c,w as l,u as e,cg as J,k as f,a as K,b as s,f as Q}from"./url.2e6a77c4.js";import{g as U}from"./i18n.48bd28ac.js";import{o as W,a as X}from"./config.9fb52765.js";import{u as Y}from"./replace-all.7cf5f327.js";import{E as Z}from"./index.c5fde138.js";import"./light-switch.b4bd549c.js";import"./dark.ddf8665a.js";import"./plugin-vue_export-helper.41ffa612.js";import"./windi.a5b0b048.js";import"./index.31f26a0f.js";import"./index.f5a869bb.js";import"./index.2bc50276.js";import"./index.ff0264f3.js";import"./index.379e80ad.js";import"./focus-trap.23f44899.js";import"./isNil.98bb3b88.js";import"./event.53b2ad83.js";import"./index.a439b087.js";import"./error.7e8331f1.js";import"./dropdown.0507a1c7.js";import"./index.fcc3ea42.js";import"./refs.4001ce17.js";import"./avatar.d0cba173.js";import"./logo-transparent.1566007e.js";import"./index.d75a71d9.js";import"./email.196eb60e.js";import"./index.8b079f71.js";import"./dev-mode.c4133b5a.js";import"./guid.c1a40312.js";import"./use-shortcuts.21ce2d8e.js";import"./userWorker.b3a6730b.js";import"./editor.main.d2800f63.js";import"./preload-helper.13a99eb0.js";import"./page.831d1a45.js";import"./validate.efc4a5c0.js";import"./index.aba77680.js";import"./index.6d45a031.js";import"./index.60af85f4.js";import"./style.19d0c187.js";import"./toNumber.574be4f1.js";import"./_baseIsEqual.c0d7e77a.js";import"./index.6855d037.js";import"./_baseClone.adbc92f5.js";import"./isEqual.f1ae9fb3.js";import"./index.99c4f65d.js";import"./index.fc90f5ad.js";import"./event.776e7e11.js";import"./index.603c1365.js";import"./scroll.f51de5d8.js";import"./aria.75ec5909.js";import"./validator.e2869aba.js";import"./use-copy-text.a346ed23.js";import"./index.c6df1b45.js";import"./index.0f940e7f.js";import"./classCompletion.a22e38a6.js";import"./vuedraggable.umd.5840ebc7.js";import"./cloneDeep.ff43c1f8.js";import"./toggleComment.5b29ca87.js";import"./use-save-tip.04d9878f.js";import"./confirm.e2c924ff.js";import"./index.a731e53b.js";import"./index.cbddf8eb.js";import"./index.695d0284.js";import"./index.0d2684bf.js";import"./debounce.61c67278.js";import"./index.064c7de9.js";import"./index.c4e9b529.js";import"./icon-button.3313e42d.js";import"./index.8262f5ef.js";import"./index.66d5d547.js";import"./index.77edae39.js";import"./index.232e741a.js";import"./plugin-vue_export-helper.21dcd24c.js";import"./index.af90dc36.js";import"./index.470f0e7e.js";import"./index.deca86b5.js";import"./alert.583ccfe6.js";import"./index.fafb14b3.js";/* empty css                                                               */import"./media-list.vue_vue_type_style_index_0_scoped_true_lang.118b18f5.js";/* empty css                                                          */import"./file.b0d4cc6e.js";import"./use-date.01b82ce0.js";import"./dayjs.min.0a66969b.js";/* empty css                                                          *//* empty css                                                                 */import"./use-file-upload.82732353.js";/* empty css                                                         */import"./image-editor.vue_vue_type_style_index_0_scoped_true_lang.f6d26366.js";import"./image-editor.846c8dc6.js";import"./main.esm.5190fb65.js";import"./index.f01d4ffd.js";import"./index.968f79fd.js";import"./diff.dece7d27.js";import"./index.50c16ae5.js";const oo={key:1,class:"h-full flex flex-col bg-[#f3f5f5] dark:bg-[#1e1e1e]"},to={class:"flex-1 overflow-hidden"},Qt=T({setup(r){const{t:o}=U(),t=V(),h=R(),S=A(),w=q(),a=z(!1),C=i=>{a.value=i};G(()=>w.query.SiteId,i=>{i&&a.value&&(O({message:o("common.siteChangingSuccess"),type:"success"}),a.value=!1)});const k=i=>{P(o("common.resourceNotFound",{url:i}))};return v(W,i=>{const m=Y({name:`${i.type.toLowerCase()}-edit`,query:g(_({},i.params),{id:i.id})}),{href:n}=L.resolve(m);J(n)}),v(X,k),(i,m)=>{const n=H("router-view"),F=Z;return p(),c(F,{locale:e(t).locale},{default:l(()=>[e(S).show?(p(),c(M,{key:0})):f("",!0),e(h).site?(p(),K("div",oo,[s(e(B),{class:"pl-40px"},{left:l(()=>[s(j,{onChange:m[0]||(m[0]=N=>C(N))}),s(D),s($)]),right:l(()=>[]),_:1}),Q("div",to,[e(t).header?(p(),c(n,{key:0})):f("",!0)])])):f("",!0)]),_:1},8,["locale"])}}});export{Qt as default};