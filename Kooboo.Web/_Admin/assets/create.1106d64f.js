var u=(n,m,t)=>new Promise((i,r)=>{var e=o=>{try{p(t.next(o))}catch(a){r(a)}},c=o=>{try{p(t.throw(o))}catch(a){r(a)}},p=o=>o.done?i(o.value):Promise.resolve(o.value).then(e,c);p((t=t.apply(n,m)).next())});import{_ as f}from"./index.f237f871.js";import{d,cd as _,g,o as h,a as B,b as s,u as l,F as b}from"./url.2e6a77c4.js";import{B as v}from"./breadcrumb.80c136b8.js";import{g as k}from"./i18n.48bd28ac.js";import{c as C}from"./shipping.f7d1d018.js";import{u as S}from"./replace-all.7cf5f327.js";import{_ as x}from"./edit-form.66d220db.js";import"./index.2bc50276.js";import"./windi.a5b0b048.js";import"./index.ff0264f3.js";import"./main.ea42d807.js";import"./index.50c16ae5.js";import"./preload-helper.13a99eb0.js";/* empty css                                                               */import"./plugin-vue_export-helper.21dcd24c.js";import"./plugin-vue_export-helper.41ffa612.js";import"./index.a439b087.js";import"./error.7e8331f1.js";import"./tooltip.28a451cc.js";import"./index.379e80ad.js";import"./focus-trap.23f44899.js";import"./isNil.98bb3b88.js";import"./event.53b2ad83.js";import"./condition.5fbdcdcf.js";import"./icon-button.3313e42d.js";import"./index.8262f5ef.js";import"./index.fcc3ea42.js";import"./dropdown-input.1a28e074.js";import"./index.f5a869bb.js";import"./dropdown.0507a1c7.js";import"./refs.4001ce17.js";import"./index.fc90f5ad.js";import"./event.776e7e11.js";import"./index.0d2684bf.js";import"./scroll.f51de5d8.js";import"./isEqual.f1ae9fb3.js";import"./_baseIsEqual.c0d7e77a.js";import"./debounce.61c67278.js";import"./toNumber.574be4f1.js";import"./index.064c7de9.js";import"./validator.e2869aba.js";import"./index.695d0284.js";import"./index.6855d037.js";import"./_baseClone.adbc92f5.js";import"./commerce.f8d3336c.js";import"./index.d4bbb472.js";import"./index.e3f90979.js";const vo=d({setup(n){const{t:m}=k(),t=_(),i=g({name:"",description:"",baseCost:10,additionalCosts:[],estimatedDaysOfArrival:3,countries:[],isDefault:!1});function r(){t.goBackOrTo(S({name:"shippings"}))}function e(){return u(this,null,function*(){yield C(i.value),r()})}return(c,p)=>{const o=f;return h(),B(b,null,[s(v,{class:"p-24","crumb-path":[{name:l(m)("common.shippings"),route:{name:"shippings"}},{name:l(m)("common.create")}]},null,8,["crumb-path"]),s(x,{model:i.value},null,8,["model"]),s(o,{permission:{feature:"shipping",action:"edit"},onCancel:r,onSave:e})],64)}}});export{vo as default};