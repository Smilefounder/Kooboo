var P=Object.defineProperty,K=Object.defineProperties;var L=Object.getOwnPropertyDescriptors;var b=Object.getOwnPropertySymbols;var A=Object.prototype.hasOwnProperty,D=Object.prototype.propertyIsEnumerable;var g=(a,e,t)=>e in a?P(a,e,{enumerable:!0,configurable:!0,writable:!0,value:t}):a[e]=t,m=(a,e)=>{for(var t in e||(e={}))A.call(e,t)&&g(a,t,e[t]);if(b)for(var t of b(e))D.call(e,t)&&g(a,t,e[t]);return a},d=(a,e)=>K(a,L(e));import{b as h,d as c,m as p,u as w,_ as $,w as v}from"./windi.a5b0b048.js";import{d as j,B as I,c as f,W as J,o as N,j as C,w as E,r as x,n as O,u as r,m as R,l as S,b2 as T}from"./url.2e6a77c4.js";import{i as _}from"./i18n.48bd28ac.js";const k=Symbol("rowContextKey"),W=h({tag:{type:String,default:"div"},span:{type:Number,default:24},offset:{type:Number,default:0},pull:{type:Number,default:0},push:{type:Number,default:0},xs:{type:c([Number,Object]),default:()=>p({})},sm:{type:c([Number,Object]),default:()=>p({})},md:{type:c([Number,Object]),default:()=>p({})},lg:{type:c([Number,Object]),default:()=>p({})},xl:{type:c([Number,Object]),default:()=>p({})}}),q={name:"ElCol"},F=j(d(m({},q),{props:W,setup(a){const e=a,{gutter:t}=I(k,{gutter:f(()=>0)}),n=w("col"),i=f(()=>{const s={};return t.value&&(s.paddingLeft=s.paddingRight=`${t.value/2}px`),s}),l=f(()=>{const s=[];return["span","offset","pull","push"].forEach(o=>{const u=e[o];_(u)&&(o==="span"?s.push(n.b(`${e[o]}`)):u>0&&s.push(n.b(`${o}-${e[o]}`)))}),["xs","sm","md","lg","xl"].forEach(o=>{_(e[o])?s.push(n.b(`${o}-${e[o]}`)):J(e[o])&&Object.entries(e[o]).forEach(([u,y])=>{s.push(u!=="span"?n.b(`${o}-${u}-${y}`):n.b(`${o}-${y}`))})}),t.value&&s.push(n.is("guttered")),s});return(s,B)=>(N(),C(S(s.tag),{class:O([r(n).b(),r(l)]),style:R(r(i))},{default:E(()=>[x(s.$slots,"default")]),_:3},8,["class","style"]))}}));var G=$(F,[["__file","/home/runner/work/element-plus/element-plus/packages/components/col/src/col.vue"]]);const se=v(G),H=["start","center","end","space-around","space-between","space-evenly"],M=["top","middle","bottom"],Q=h({tag:{type:String,default:"div"},gutter:{type:Number,default:0},justify:{type:String,values:H,default:"start"},align:{type:String,values:M,default:"top"}}),U={name:"ElRow"},V=j(d(m({},U),{props:Q,setup(a){const e=a,t=w("row"),n=f(()=>e.gutter);T(k,{gutter:n});const i=f(()=>{const l={};return e.gutter&&(l.marginRight=l.marginLeft=`-${e.gutter/2}px`),l});return(l,s)=>(N(),C(S(l.tag),{class:O([r(t).b(),r(t).is(`justify-${e.justify}`,l.justify!=="start"),r(t).is(`align-${e.align}`,l.align!=="top")]),style:R(r(i))},{default:E(()=>[x(l.$slots,"default")]),_:3},8,["class","style"]))}}));var X=$(V,[["__file","/home/runner/work/element-plus/element-plus/packages/components/row/src/row.vue"]]);const ae=v(X);export{se as E,ae as a};